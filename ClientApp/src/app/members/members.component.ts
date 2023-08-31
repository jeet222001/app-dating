import { Component, OnInit } from '@angular/core';
import { Member } from '../_models/member';
import { MembersService } from '../_services/members.service';
import { Pagination } from '../_models/pagination';
import { AccountService } from '../_services/account.service';
import { take } from 'rxjs';
import { UserParams } from '../_models/userParams';
import { User } from '../_models/user';

@Component({
  selector: 'app-members',
  templateUrl: './members.component.html',
  styleUrls: ['./members.component.css']
})
export class MembersComponent implements OnInit {
  members: Member[] = [];
  Pagination: Pagination | undefined
  userParams: UserParams | undefined;
  user: User | undefined;
  constructor(private memberService: MembersService,
    private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        if (user) {
          this.userParams = new UserParams(user);
          this.user = user;
        }
      }
    })
  }
  ngOnInit(): void {
    this.loadMembers();
  }

  loadMembers() {
    if (!this.userParams) return;
    this.memberService.getMembers(this.userParams).subscribe({
      next: response => {
        if (response.result && response.Pagination) {
          this.members = response.result
          this.Pagination = response.Pagination
        }
      }
    })
  }
}
