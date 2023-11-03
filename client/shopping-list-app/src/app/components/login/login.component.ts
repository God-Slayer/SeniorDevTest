import { Component, OnInit } from '@angular/core';
import { LoginService } from 'src/app/services/login/login.service';
import {
  FacebookAuthResponse,
  LoginAuthResponse,
} from 'src/app/interfaces/ILoginService';
import { Router } from '@angular/router';

declare const FB: any;

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.less'],
})
export class LoginComponent implements OnInit {
  constructor(private loginService: LoginService, private router: Router) {}

  ngOnInit(): void {
    sessionStorage.clear();
  }

  async LoginWithFacebook() {
    FB.login(
      async (result: FacebookAuthResponse) => {
        if (result.authResponse != null && result.status == 'connected') {
          this.loginService
            .LoginWithFacebook(result.authResponse.accessToken)
            .subscribe(
              (response: any) => {
                if (response.success && response.message == 'Authorized') {
                  if (response.data) {
                    this.loginService.SetFbUserData(response.data);
                    this.loginService.SetUserLoginState(true);
                    this.router.navigate(['/shopping-list']);
                  }
                }
              },
              (error: any) => {
                console.log(error);
              }
            );
        }
      },
      { scope: 'email' }
    );
  }
}
