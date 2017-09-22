import { IJsonUser } from "./jsonuser.interface";

export class User {
    emailAddress: string;
    firstName: string;
    lastName: string;

    static fromJson(jsonUser: IJsonUser): User {
        let newUser = new User();

        newUser.emailAddress = jsonUser.emailAddress;
        newUser.firstName = jsonUser.firstName;
        newUser.lastName = jsonUser.lastName;

        return newUser;
    }

    get fullName(): string {
        return this.firstName + " " + this.lastName;
    }
}