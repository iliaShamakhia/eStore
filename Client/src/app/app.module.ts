import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app.component';
import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { MainComponent } from './main/main.component';
import { Routes, RouterModule} from '@angular/router';
import { GameDetailsComponent } from './game-details/game-details.component';
import { AddGameComponent } from './add-game/add-game.component';
import { FormsModule } from '@angular/forms';
import { EditGameComponent } from './edit-game/edit-game.component';
import { RegisterUserComponent } from './register-user/register-user.component';
import { JwtModule } from "@auth0/angular-jwt";
import { CommentComponent } from './comment/comment.component';
import { CartComponent } from './cart/cart.component';
import { CompleteOrderComponent } from './complete-order/complete-order.component';
import { AdminPanelComponent } from './admin-panel/admin-panel.component';

const routes : Routes = [
  {
    path : '',
    component : MainComponent
  },
  {
    path : 'games',
    component : MainComponent
  },
  {
    path : 'games/:id',
    component : GameDetailsComponent
  },
  {
    path : 'add-game',
    component : AddGameComponent
  },
  {
    path : 'edit-game/:id',
    component : EditGameComponent
  },
  {
    path : 'register',
    component : RegisterUserComponent
  },
  {
    path : 'cart',
    component : CartComponent
  },
  {
    path : 'complete-order',
    component : CompleteOrderComponent
  },
  {
    path : 'admin-panel',
    component : AdminPanelComponent
  }
];

export function tokenGetter() { 
  return localStorage.getItem("jwt"); 
}

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    FooterComponent,
    MainComponent,
    GameDetailsComponent,
    AddGameComponent,
    EditGameComponent,
    RegisterUserComponent,
    CommentComponent,
    CartComponent,
    CompleteOrderComponent,
    AdminPanelComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    RouterModule.forRoot(routes),
    FormsModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: [],
        disallowedRoutes: [],
      },
    })
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
