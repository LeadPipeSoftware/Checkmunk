import { Component, Inject, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { Observable } from 'rxjs/Rx';
import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/throttleTime';
import 'rxjs/add/observable/fromEvent';
import { ReactiveFormsModule } from '@angular/forms';

import { UserService } from '../../services/user.service';

import { User } from '../../models/user';

@Component({
    selector: 'users',
    templateUrl: './users.component.html',
    providers: [UserService]
})

export class UsersComponent {
    errorMessage: string;
    filterInput = new FormControl();
    filterText: string;
    filterPlaceholder: string;
    users: User[] = [];

    constructor(private userService: UserService) {
    }

    ngOnInit() {
        this.initializeFilter();
        this.getUsersFromApi();
    }

    private getUsersFromApi() {
        this.userService
            .getAllUsers()
            .then(users => {
                this.errorMessage = "";
                this.users = users;
            })
            .catch((error) => {
                this.errorMessage = error;
                console.log(error);
            });
    }

    private initializeFilter() {
        this.filterText = "";
        this.filterPlaceholder = "Filter...";

        this.filterInput
            .valueChanges
            .debounceTime(200)
            .subscribe(term => {
                this.filterText = <string>(term);
                console.log(term);
            });
    }
}
