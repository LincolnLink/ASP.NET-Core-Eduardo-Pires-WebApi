import { Component, OnInit } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '../userService';
import { User } from '../user';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html'
})
export class LoginComponent implements OnInit {
  userForm: UntypedFormGroup;
  user: User;
  errors: any[] = [];

  constructor(private fb: UntypedFormBuilder,
    private router: Router,
    private userService: UserService) { }

  ngOnInit() {
    this.userForm = this.fb.group({
      email: '',
      password: ''
    });
  }

  login() {
    if (this.userForm.valid && this.userForm.dirty) {

      let _user = Object.assign({}, this.user, this.userForm.value);

      this.userService.login(_user)
        .subscribe(
          result => { this.onSaveComplete(result) },
          fail => { this.onError(fail) }
        );
    }
  }

  onSaveComplete(response: any) {
    this.userService.persistirUserApp(response);
    this.router.navigateByUrl('/lista-produtos');
  }

  onError(fail: any) {
    console.log('erro console:', fail.error);
    this.errors = fail.error?.errors ? fail.error?.errors  : [] ;
  }
}
