import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { FbUserData, ILoginService } from 'src/app/interfaces/ILoginService';

@Injectable({
  providedIn: 'root',
})
export class LoginService implements ILoginService {
  private loggedIn: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(
    false
  );

  private userData: BehaviorSubject<FbUserData> =
    new BehaviorSubject<FbUserData>({});

  constructor(private httpClient: HttpClient) {}

  LoginWithFacebook(credentials: any): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    return this.httpClient.post<any>(
      '/api/Login',
      JSON.stringify(credentials),
      {
        headers: headers,
        withCredentials: true,
      }
    );
  }

  GetUserLoginState(): BehaviorSubject<boolean> {
    var ss = sessionStorage.getItem('shoppingListAppLoggedInState');
    if (ss != null || ss != undefined) {
      this.loggedIn.next(JSON.parse(ss!));
    }
    return this.loggedIn;
  }

  SetUserLoginState(value: boolean): void {
    this.loggedIn.next(value);

    sessionStorage.setItem(
      'shoppingListAppLoggedInState',
      JSON.stringify(value)
    );
  }

  GetFbUserData(): BehaviorSubject<FbUserData> {
    var ss = sessionStorage.getItem('shoppingListAppFbUserData');

    if (ss != null || ss != undefined) {
      this.userData.next(JSON.parse(ss!));
    }

    return this.userData;
  }

  SetFbUserData(value: FbUserData): void {
    this.userData.next(value);

    sessionStorage.setItem('shoppingListAppFbUserData', JSON.stringify(value));
  }
}
