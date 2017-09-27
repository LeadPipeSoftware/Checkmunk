import { Injectable } from '@angular/core';
import { Http, Headers, Response } from '@angular/http';

import 'rxjs/add/operator/toPromise';

import { User } from '../models/user';
import { IJsonUser } from '../models/jsonuser.interface';
import { SettingsService } from './settings.service';

// require hack
declare function require(path: string): any;

// require untyped library file
var structuredLog = require('structured-log');

@Injectable()
export class UserService {
    private log: any;

    constructor(private settingsService: SettingsService, private http: Http) {
        this.log = structuredLog.configure()
            .minLevel.debug()
            .writeTo(new structuredLog.ConsoleSink())
            .create();
    }

    create(user: User): Promise<User> {
        const headers = new Headers({
            'Content-Type': "application/json",
        });

        this.log.info('POST {User}', user);

        const url = this.settingsService.apiUrl + "api/v1/Users"; // This is terrible. Use https://github.com/jfromaniello/url-join instead.

        return this.http
            .post(url, JSON.stringify({ firstName: user.firstName, lastName: user.lastName }), { headers: headers })
            .toPromise()
            .then(result => result.json() as User)
            .catch(error => {
                if (error.status === 409) {
                    return Promise.reject("A user with that email address already exists");
                }

                this.log.error(error, error.message);
                return Promise.reject(error.message || error);
            });
    }

    getAllUsers(): Promise<User[]> {
        const url = this.settingsService.apiUrl + "api/v1/Users"; // This is terrible. Use https://github.com/jfromaniello/url-join instead.

        this.log.info('GET {Url}', url);

        return this.http
            .get(url)
            .toPromise()
            .then(this.extractData)
            .catch(error => {
                this.log.info('Uh-oh! {Error}', error);
                this.log.error(error, error.message);
                return Promise.reject(error.message || error);
            });
    }

    private extractData(response: Response) {
        let jsonUsers: IJsonUser[] = response.json() as IJsonUser[];
        let users: User[] = [];

        for (let i = 0; i < jsonUsers.length; i++) {
            users.push(User.fromJson(jsonUsers[i]));
        }

        return users;
    }

    // DEPRECATED
    private createGuid(): string {
        function random() {
            return Math.floor((1 + Math.random()) * 0x10000)
                .toString(16)
                .substring(1);
        }

        return random() + random() + '-' + random() + '-' + random() + '-' + random() + '-' + random() + random() + random();
    }
}