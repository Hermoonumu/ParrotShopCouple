using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParrotShopBackend.Application.Services;
using ParrotShopBackend.Domain;
using Stripe;

namespace ParrotShopBackend.API;


[ApiController]
[Route("/api/payments")]

public class PaymentController(IUserService _userSvc, IConfiguration _conf, ICheckoutService _chkSvc) : ControllerBase
{
    [HttpPost("newPaymentIntent")]
    [Authorize]
    public async Task<IActionResult> NewPaymentIntent()
    {
        User? user = await _userSvc.GetUserByToken(User, true);
        if (user.Cart.CartItems.Count() <= 0)
        {
            return BadRequest(new { message = "Cart is empty" });
        }
        long total = user.Cart.CartItems.Sum(x => (long)x.Item.Price);

        var options = new PaymentIntentCreateOptions
        {
            Amount = total * 100,
            Currency = "usd",
            Metadata = new Dictionary<string, string>
            {
                {"CartId", user.Cart.Id.ToString()},
                {"UserId", user.Id.ToString()}
            }
        };

        var service = new PaymentIntentService();
        try
        {
            PaymentIntent intent = service.Create(options);
            return Ok(new { clientSecret = intent.ClientSecret });
        }
        catch (StripeException e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    [HttpPost("webhook")]
    public async Task<IActionResult> StripeWebhook()
    {
        // 1. Read the raw request body Stripe sends
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

        try
        {
            // 2. Verify the signature using your endpoint secret
            var stripeEvent = EventUtility.ConstructEvent(
                json,
                Request.Headers["Stripe-Signature"],
                _conf["Stripe:WebhookSecret"]
            );

            // 3. Handle the specific event we care about
            if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
            {
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

                // 4. Extract the Metadata we attached earlier!
                var cartId = paymentIntent.Metadata["CartId"];
                var userId = long.Parse(paymentIntent.Metadata["UserId"]);


                Console.WriteLine($"Payment for Cart {cartId} succeeded! Amount: {paymentIntent.Amount}");

                // TODO: Connect to your PostgreSQL database context here.
                // - Find the order associated with cartId
                // - Mark the order status as "Paid"
                // - Reserve the parrot in the inventory
                // - Send a confirmation email

                await _chkSvc.CheckoutAsync(await _userSvc.GetUserByIdAsync(userId, true));

            }
            else if (stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
            {
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                Console.WriteLine($"Payment failed: {paymentIntent.LastPaymentError?.Message}");
                // Handle failure (e.g., notify user)
            }

            // 5. Always return a 200 OK so Stripe knows you received it
            return Ok();
        }
        catch (StripeException e)
        {
            Console.WriteLine($"Stripe webhook error: {e.Message}");
            return BadRequest(); // Tells Stripe something went wrong
        }
    }
}
