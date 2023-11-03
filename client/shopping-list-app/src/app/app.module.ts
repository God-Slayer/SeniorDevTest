import { ShoppingListService } from "./services/shopping-list/shopping-list.service";
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { ShoppingListComponent } from './components/shopping-list/shopping-list.component';
import { LoginService } from "./services/login/login.service";

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    ShoppingListComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule    
  ],
  providers: [
    LoginService,
    ShoppingListService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
