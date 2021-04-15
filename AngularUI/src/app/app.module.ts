import { Component, OnChanges, OnInit, Input, NgModule, NgModuleFactory, Compiler, SimpleChanges, NO_ERRORS_SCHEMA } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { DataTablesModule } from 'angular-datatables';
import { AppComponent } from './app.component';
import { DepartmentComponent } from './department/department.component';
import { ShowDepartmentComponent } from './department/show-department/show-department.component';
import { AddEditDepartmentComponent } from './department/add-edit-department/add-edit-department.component';
import { EmployeeComponent } from './employee/employee.component';
import { ShowEmployeeComponent } from './employee/show-employee/show-employee.component';
import { AddEditEmployeeComponent } from './employee/add-edit-employee/add-edit-employee.component';
import { EmployeeService } from './employee.service';
import { DepartmentService } from './department.service';

import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DatePipe } from '@angular/common';
import * as $ from "jquery";


@NgModule({
  declarations: [
    AppComponent,
    DepartmentComponent,
    ShowDepartmentComponent,
    AddEditDepartmentComponent,
    EmployeeComponent,
    ShowEmployeeComponent,
    AddEditEmployeeComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    DataTablesModule
  ],
  providers: [EmployeeService, DepartmentService, DatePipe],
  bootstrap: [AppComponent]
})
export class AppModule { 
}
