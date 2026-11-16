import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root',
})
export class Token {
  private accessTokenKey = 'accessToken';
  private refreshTokenKey = 'refreshToken';

  saveTokens(accessToken:string,refreshToken:string):void{
    sessionStorage.setItem(this.accessTokenKey,accessToken);
    sessionStorage.setItem(this.refreshTokenKey,refreshToken);
  }

  getAccessToken():string | null{
    return sessionStorage.getItem(this.accessTokenKey);
  }

  getRefreshToken():string | null{
    return sessionStorage.getItem(this.refreshTokenKey);
  }

  getUserRole(): string | null{
    const token = this.getAccessToken();

    if(!token){
      return null;
    }
    try{
      const decoded:any = jwtDecode(token);
       return decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ?? null;
    }
    catch{
      return null;
    }
  }

  clearTokens():void{
    sessionStorage.removeItem(this.accessTokenKey);
    sessionStorage.removeItem(this.refreshTokenKey);
  }
}
