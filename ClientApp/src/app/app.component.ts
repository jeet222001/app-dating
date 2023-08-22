import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'Dating App';
  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
    this.setCurrentUser();
  }


  setCurrentUser() {
    const userstr = localStorage.getItem('User');
    if (!userstr) return;
    const user: User = JSON.parse(userstr);
    this.accountService.setCurrentUser(user);
  }
}