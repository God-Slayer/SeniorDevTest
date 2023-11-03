import { Injectable } from '@angular/core';
import {
  IShoppingListService,
  ShoppingListResponse,
} from 'src/app/interfaces/IShoppingListService';
import { HttpClient, HttpHeaders, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API } from 'src/app/constants/ShoppingList.Constants';

@Injectable({
  providedIn: 'root',
})
export class ShoppingListService implements IShoppingListService {
  headers = new HttpHeaders({
    'Content-Type': 'application/json',
  });

  constructor(private httpClient: HttpClient) {}

  GetShoppingList(userId: string): Observable<ShoppingListResponse[]> {
    return this.httpClient.get<ShoppingListResponse[]>(
      `${API.Endpoints.ShoppingList}${userId}`,
      {
        headers: this.headers,
      }
    );
  }

  AddToShoppingList(payload: ShoppingListResponse): Observable<any> {
    var response: any = this.httpClient.post<any>(
      `${API.Endpoints.ShoppingList}`,
      payload,
      {
        headers: this.headers,
      }
    );

    return response;
  }

  EditShoppingList(payload: ShoppingListResponse): Observable<any> {
    var response: any = this.httpClient.put<any>(
      `${API.Endpoints.ShoppingList}`,
      payload,
      {
        headers: this.headers,
      }
    );

    return response;
  }

  RemoveFromShoppingList(index: any): Observable<any> {
    var response = this.httpClient.delete<any>(
      `${API.Endpoints.ShoppingList}${index}`,
      {
        headers: this.headers,
      }
    );
    return response;
  }

  UploadImage(image: File, payload: ShoppingListResponse): Observable<any> {
    const formData = new FormData();
    formData.append('image', image);
    formData.append('index', payload.index?.toString()!);
    formData.append('item', payload.item!);
    formData.append('quantity', payload.quantity?.toString()!);
    formData.append('userId', payload.userId!);

    let headers = new HttpHeaders({
      'Content-Type': 'multipart/form-data',
      enctype: 'multipart/form-data',
    });

    return this.httpClient.post<any>(
      `${API.Endpoints.ShoppingList}ImageUpload`,
      formData,
      {
        headers,
      }
    );
  }
}
