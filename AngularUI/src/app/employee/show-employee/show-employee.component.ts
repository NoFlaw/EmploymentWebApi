import { Component, OnInit } from '@angular/core';
import { EmployeeService } from 'src/app/employee.service';
import { Pipe, PipeTransform } from '@angular/core';
import * as $ from "jquery"; 
import { Subject } from 'rxjs';

@Component({
  selector: 'app-show-employee',
  templateUrl: './show-employee.component.html',
  styleUrls: ['./show-employee.component.css']
})
export class ShowEmployeeComponent implements OnInit {  
  Employee:any;
  EmployeeList:any=[];

  ModalTitle:string = "";
  ActivateAddEditEmployeeComponent:boolean = false;

  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<any> = new Subject<any>();

  constructor(private employeeService: EmployeeService ) { }

  ngOnInit(): void {
   
    this.dtOptions = {
      pagingType: 'full_numbers',
      pageLength: 10
    };

    this.Employee = {
      EmployeeId: 0,
      EmployeeName: "",
      Department:"",
      StartDate:"",
      PhotoFileName:"anonymous.png"
    };

    this.refreshEmployeeList();
  }

  addClick(){
    this.Employee = {
      EmployeeId: 0,
      EmployeeName: "",
      Department:"",
      StartDate:"",
      PhotoFileName:"anonymous.png"
    };

    this.ModalTitle = "Add Employee";
    this.ActivateAddEditEmployeeComponent = true;
  }

  editClick(item: any) {  
    this.Employee = item;    
    this.ModalTitle = "Edit Employee"
    this.ActivateAddEditEmployeeComponent = true;
  }

  deleteClick(item: any) {
    if (confirm('Are you sure you want to remove this Employee?')){
      this.employeeService.deleteEmployee(item.EmployeeId).subscribe(response => {
        console.log(response);
        alert("Success - Employee Removed");
        this.refreshEmployeeList();
      },
      error => {
        console.log(error);
        alert("Error - Unable to Remove Employee - employeeService.deleteEmployee");
      });
    }
  } 

  closeClick() {
    this.dtTrigger.unsubscribe();
    this.ActivateAddEditEmployeeComponent = false;
    this.refreshEmployeeList();
  }

  refreshEmployeeList(){
    this.employeeService.getEmployeeList().subscribe(data => {
      this.EmployeeList = data;
      this.dtTrigger.next();
    });
  }

  ngOnDestroy(): void {
    // Do not forget to unsubscribe the event
    this.dtTrigger.unsubscribe();
  }
}
