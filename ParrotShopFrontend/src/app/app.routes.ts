import { Routes } from '@angular/router';
import { Home } from './home/home';
import { LoginRegister } from './login-register/login-register';
import { Orders } from './orders/orders';
import { authGuard } from './auth-guard';
import { AccountManagement } from './account-management/account-management';
import { NotFound } from './not-found/not-found';
import { ParrotQuestionnaire } from './parrot-questionnaire/parrot-questionnaire';
import { ParrotMgmt } from './parrot-mgmt/parrot-mgmt';
import { adminguardGuard } from './adminguard-guard';
import { Search } from './search/search';
import { ParrotShow } from './parrot-show/parrot-show';
import { Cart } from './cart/cart';
import { Checkout } from './checkout/checkout';
import { CheckoutSuccess } from './checkout-success/checkout-success';
import { checkoutGuard } from './checkout-guard';
import { QuizResDisplay } from './quiz-res-display/quiz-res-display';
import { Browse } from './browse/browse';

export const routes: Routes = [
    {path:'', component: Home},
    {path:'login', component: LoginRegister},
    {path:'register', component: LoginRegister},
    {path:'orders', component: Orders, canActivate: [authGuard]},
    {path:'accmgmt', component: AccountManagement, canActivate: [authGuard]},
    {path: 'parrotQuestionnaire', component:ParrotQuestionnaire},
    {path: 'manage', component:ParrotMgmt, canActivate: [adminguardGuard]},
    {path: 'search', component:Search},
    {path: 'parrot/:id', component:ParrotShow},
    {path: 'cart', component: Cart, canActivate: [authGuard]},
    {path: 'checkout', component:Checkout, canActivate:[checkoutGuard]},
    {path: 'orders', component:Orders, canActivate: [authGuard]},
    {path: 'checkoutSuccess', component:CheckoutSuccess},
    {path: 'recommendedParrots', component:QuizResDisplay},
    {path: 'browse', component:Browse},
    {path:'**', component: NotFound}
];
