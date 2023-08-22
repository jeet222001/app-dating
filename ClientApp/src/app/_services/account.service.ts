import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { User } from 'src/app/_models/user';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) { }

  login(model: any) {
    debugger
    return this.http.post<User>(this.baseUrl + 'account/login', model)
      .pipe(map(((response: User) => {
        console.log(model);

        const user = response;
        if (user) {
          this.setCurrentUser(user);
        }
      })));
  }
  setCurrentUser(user: User) {
    localStorage.setItem('User', JSON.stringify(user));
    this.currentUserSource.next(user);

  }
  logout() {
    localStorage.removeItem('User');
    this.currentUserSource.next(null);
  }

  Register(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
      map(
        (user: User) => {
          if (user) {
            this.setCurrentUser(user);
          }
          return user
        }
      )
    )
  }
}
