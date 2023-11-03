import { FbUserData } from './../../interfaces/ILoginService';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import {
  ApiResponse,
  ShoppingListResponse,
} from 'src/app/interfaces/IShoppingListService';
import { LoginService } from 'src/app/services/login/login.service';
import { ShoppingListService } from 'src/app/services/shopping-list/shopping-list.service';

@Component({
  selector: 'app-shopping-list',
  templateUrl: './shopping-list.component.html',
  styleUrls: ['./shopping-list.component.less'],
})
export class ShoppingListComponent implements OnInit {
  shoppingList: ShoppingListResponse[];
  userData: FbUserData;
  selectedFile: File | null = null;

  constructor(
    private loginService: LoginService,
    private router: Router,
    private shoppingListService: ShoppingListService
  ) {}

  ngOnInit(): void {
    this.loginService.GetUserLoginState().subscribe((loginState) => {
      if (!loginState) {
        this.router.navigate(['/login']);
      }
    });

    this.loginService.GetFbUserData().subscribe((data: FbUserData) => {
      this.userData = data;

      if (this.userData.id) {
        this.shoppingListService
          .GetShoppingList(this.userData.id)
          .subscribe((response: ShoppingListResponse[]) => {
            this.shoppingList = response;
          });
      }
    });
  }

  Modify(item: string, quantity: string, method: string, index?: any) {
    if (item.trim() == '' || item.trim().length < 1) {
      alert('Item name cannot be blank');
    }

    switch (method) {
      case 'add':
        var payload: ShoppingListResponse = {
          userId: this.userData.id,
          item,
          quantity: parseInt(quantity),
        };

        this.shoppingListService.AddToShoppingList(payload).subscribe(
          (response: ApiResponse) => {
            if (response.success) {
              this.shoppingList.push({
                index: parseInt(`${response.data}`),
                userId: this.userData.id,
                item,
                quantity: parseInt(quantity),
              });
            }
          },
          (error) => {
            console.log(error);
          }
        );
        break;
      case 'update':
        var payload: ShoppingListResponse = {
          index,
          item,
          quantity: parseInt(quantity),
        };

        this.shoppingListService.EditShoppingList(payload).subscribe(
          (response: ApiResponse) => {
            if (response.success) {
              this.shoppingList = this.shoppingList.map((obj) => {
                if (obj.index === index) {
                  return {
                    ...obj,
                    item: item,
                    quantity: parseInt(quantity),
                  };
                }
                return obj;
              });
              alert('Successfully updated');
            }
          },
          (error) => {
            console.log(error);
          }
        );
        break;

      default:
        this.shoppingListService.RemoveFromShoppingList(index).subscribe(
          (response: ApiResponse) => {
            if (response.success) {
              this.shoppingList = this.shoppingList.filter((x) => {
                return x.index != index;
              });
            }
          },
          (error) => {
            console.log(error);
          }
        );
        break;
    }
  }
}
