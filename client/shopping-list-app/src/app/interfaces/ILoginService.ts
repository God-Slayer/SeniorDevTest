import { BehaviorSubject, Observable } from 'rxjs';

export interface ILoginService {
  LoginWithFacebook(credentials: any): Observable<any>;

  GetUserLoginState(): BehaviorSubject<boolean>;

  SetUserLoginState(value: boolean): void;

  GetFbUserData(): BehaviorSubject<FbUserData>;

  SetFbUserData(value: FbUserData): void;
}

export interface FacebookAuthResponse {
  authResponse: AuthResponse;
  status: string;
}

export interface AuthResponse {
  accessToken: string;
  userID: string;
  expiresIn: number;
  signedRequest: string;
  graphDomain: string;
  data_access_expiration_time: number;
}

export interface LoginAuthResponse {
  success?: boolean;
  message?: string;
  data?: FbUserData;
}

export interface FbUserData {
  id?: string;
  firstName?: string;
  lastName?: string;
  email?: any;
}
