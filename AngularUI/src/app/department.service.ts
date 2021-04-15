import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class DepartmentService {

  readonly ApiUrl = "http://localhost:42991/api/Department";

  constructor(private http: HttpClient) { }

  getDepartmentList():Observable<any[]>{
    return this.http.get<any>(this.ApiUrl + '/GetDepartmentsAsync')
    .pipe(catchError(this.handleError));
  }

  addDepartment(value:any){
    return this.http.post(this.ApiUrl + '/AddDepartmentAsync', value)
    .pipe(catchError(this.handleError));;
  }

  updateDepartment(value:any){
    return this.http.put(this.ApiUrl + '/UpdateDepartmentAsync', value)
    .pipe(catchError(this.handleError));;
  }

  deleteDepartment(value:any){
    return this.http.delete(this.ApiUrl + '/DeleteDepartmentAsync/' + value)
    .pipe(catchError(this.handleError));;
  }

  private handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      console.error('An error occurred:', error.error.message);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong.
      console.error(
        `Backend returned code ${error.status}, ` +
        `body was: ${error.error}`);
    }
    // Return an observable with a user-facing error message.
    return throwError(
      'Something bad happened; please try again later.');
  }

}