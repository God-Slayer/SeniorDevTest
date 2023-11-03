import { Observable } from 'rxjs';

export interface IShoppingListService {
  GetShoppingList(userId: string): Observable<ShoppingListResponse[]>;

  AddToShoppingList(payload: ShoppingListResponse): Observable<any>;

  EditShoppingList(payload: ShoppingListResponse): Observable<any>;

  RemoveFromShoppingList(index: any): Observable<any>;

  UploadImage(imageFile: File, payload: ShoppingListResponse): Observable<any>;
}

export interface ShoppingListResponse {
  index?: number;
  userId?: string;
  item?: string;
  quantity?: number;
}

export interface ApiResponse {
  data?: string;
  success?: boolean;
}
