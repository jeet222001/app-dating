import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  users: any;
  constructor() {

  }

  ngOnInit(): void {
  }
  RegisterMode = false;

  RegisterToggle() {
    this.RegisterMode = true;
  }

  CancelRegister(event: boolean) {
    this.RegisterMode = event
  }
}
