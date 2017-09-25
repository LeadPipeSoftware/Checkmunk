import { Injectable } from '@angular/core';
import { Http, Headers, Response } from '@angular/http';

import 'rxjs/add/operator/toPromise';

import { User } from '../models/user';
import { IJsonUser } from '../models/jsonuser.interface';

@Injectable()
export class UserService {

    constructor(private http: Http) {
    }

    create(user: User): Promise<User> {
        const headers = new Headers({
            'Content-Type': "application/json"
        });

        //const url = "https://localhost:55556/api/v1/Users";

        const url = "http://checkmunk-api.azurewebsites.net/api/v1/Users";

        return this.http
            .post(url, JSON.stringify({ firstName: user.firstName, lastName: user.lastName }), { headers: headers })
            .toPromise()
            .then(result => result.json() as User)
            .catch(error => {
                if (error.status === 409) {
                    return Promise.reject("A user with that id already exists.");
                }

                return this.handleError(error);
            });
    }

    getAllUsers(): Promise<User[]> {
        //const url = `http://localhost:55556/api/v1/Users`;
        const url = "http://checkmunk-api.azurewebsites.net/api/v1/Users";

        return this.http.get(url)
            .toPromise()
            .then(response => {
                let jsonUsers: IJsonUser[] = response.json() as IJsonUser[];
                let users: User[] = [];

                for (let i = 0; i < jsonUsers.length; i++) {
                    users.push(User.fromJson(jsonUsers[i]));
                }

                return users;
            })
            .catch(this.handleError);
    }

    private handleError(error: any): Promise<any> {
        console.error('An error occurred', error);
        return Promise.reject("Uh-oh! An error occurred.");
    }
}
