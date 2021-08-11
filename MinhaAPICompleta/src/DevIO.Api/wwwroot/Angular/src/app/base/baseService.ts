import { HttpHeaders } from '@angular/common/http';
import { throwError } from 'rxjs';

export abstract class BaseService {

    protected UrlServiceV1: string = "https://localhost:5001/api/";
    //protected UrlServiceV1: string = "http://localhost:36787/api/";
    //protected UrlServiceV1: string = "https://localhost:36787/api/v1/";
    //protected UrlServiceV1: string = "https://devioapi.azurewebsites.net/api/v1/";

    protected ObterHeaderFormData() {
        return {
            headers: new HttpHeaders({
                'Content-Disposition': 'form-data; name="produto"',
                'Authorization': `Bearer ${this.obterTokenUsuario()}`
            })
        };
    }

    protected ObterHeaderJson() {
        return {
            headers: new HttpHeaders({
                'Content-Type': 'application/json'
            })
        };
    }

    protected ObterAuthHeaderJson(){
        return {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                'X-Requested-With': 'XMLHttpRequest',
                'MyClientCert': '',        // This is empty
                'MyToken': ''   ,        // This is empty ,
                'Authorization': `Bearer ${this.obterTokenUsuario()}`,
                'Access-Control-Allow-Origin': '*',
                'Access-Control-Allow-Methods': 'GET, POST, OPTIONS, PUT, PATCH, DELETE',
                'Access-Control-Allow-Headers':'Access-Control-Allow-Headers, Origin,Accept, X-Requested-With, Content-Type, Access-Control-Request-Method, Access-Control-Request-Headers',

            })
        };
    }

    protected extractData(response: any) {
        return response.data || {};
    }

    public obterUsuario() {
        return JSON.parse(localStorage.getItem('app.user'));
    }

    protected obterTokenUsuario(): string {
        return localStorage.getItem('app.token');
    }

    protected serviceError(error: Response | any) {
        let errMsg: string;

        if (error instanceof Response) {

            errMsg = `${error.status} - ${error.statusText || ''}`;
        }
        else {
            errMsg = error.message ? error.message : error.toString();
        }

        console.error(errMsg);
        return throwError(errMsg);
    }
}
