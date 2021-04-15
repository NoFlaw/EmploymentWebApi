//import { Component, OnChanges, OnInit, Input, NgModule, NgModuleFactory, Compiler, SimpleChanges, NO_ERRORS_SCHEMA } from '@angular/core';
import { Component, OnInit, Input } from '@angular/core';
import { DepartmentService } from 'src/app/department.service';

@Component({
  selector: 'app-add-edit-department',
  templateUrl: './add-edit-department.component.html',
  styleUrls: ['./add-edit-department.component.css']
})

export class AddEditDepartmentComponent implements OnInit {

  @Input()
  Department:any;
  DepartmentId:string = "";
  DepartmentName:string = "";
  IsActive:boolean = true;

  constructor(private departmentService: DepartmentService) { }

  ngOnInit(): void {
    this.DepartmentId = this.Department.DepartmentId;
    this.DepartmentName = this.Department.DepartmentName;
    this.IsActive = this.Department.IsActive;
  }

  addDepartment(){
    var jsonObject = {DepartmentId:this.DepartmentId, DepartmentName:this.DepartmentName, IsActive:this.IsActive};
    this.departmentService.addDepartment(jsonObject).subscribe(response => {
      console.log(response);
      alert("Success - Department Added");
    },
    error => {
      console.log(error);
      alert("Error");
    });
  }

  updateDepartment(){
    var jsonObject = {DepartmentId:this.DepartmentId, DepartmentName:this.DepartmentName, IsActive:this.IsActive};
    this.departmentService.updateDepartment(jsonObject).subscribe(response => {
      console.log(response);
      alert("Success - Department Updated");
    },
    error => {
      console.log(error);
      alert("Error");
    });
  }

}
