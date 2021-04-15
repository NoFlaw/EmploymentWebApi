import { Component, OnInit, Input } from '@angular/core';
import { DatePipe } from '@angular/common'
import { EmployeeService } from 'src/app/employee.service';
import { DepartmentService } from 'src/app/department.service';
import { fromEventPattern } from 'rxjs';

@Component({
  selector: 'app-add-edit-employee',
  templateUrl: './add-edit-employee.component.html',
  styleUrls: ['./add-edit-employee.component.css']
})
export class AddEditEmployeeComponent implements OnInit {
  
  @Input()
  Employee:any;
  EmployeeId:string = "";
  EmployeeName:string = "";
  Department:any;
  DepartmentId:number = 0;
  DepartmentName:string ="";
  StartDate:any = "";
  PhotoFileName:string = "";
  PhotoFilePath:string = "";

  DepartmentsList:any=[];

  constructor(private employeeService: EmployeeService, private departmentService: DepartmentService, private datePipe: DatePipe) { }

  ngOnInit(): void {
    this.loadDepartmentList();
  }

  loadDepartmentList() {
      this.departmentService.getDepartmentList().subscribe((data:any) => {
      console.log(data);
      this.DepartmentsList = data;
      
      this.EmployeeId = this.Employee.EmployeeId;
      this.EmployeeName = this.Employee.EmployeeName;
      this.Department = this.Employee.Department;
      this.DepartmentId = this.Employee.DepartmentId;
      this.DepartmentName = this.Employee.Department.DepartmentName;
      this.StartDate = this.datePipe.transform(this.Employee.StartDate, 'yyyy-MM-dd');
      this.PhotoFileName = this.Employee.PhotoFileName;
      this.PhotoFilePath = this.employeeService.PhotoUrl + this.PhotoFileName;
    },
    error => {
      console.log(error);
      alert("Error - Unable to retrieve Departments - getAllDepartmentNames");
    });
  }

  addEmployee(){
    var employee = {
      EmployeeId:this.EmployeeId, 
      DepartmentId:this.DepartmentId,
      EmployeeName:this.EmployeeName,
      StartDate:this.StartDate,
      PhotoFileName:this.PhotoFileName,
      Department:null
    };

    this.employeeService.addEmployee(employee).subscribe(response => {
      console.log(response);
      alert("Success - Employee Added");
    },
    error => {
      console.log(error);
      alert("Error - Unable to Add Employee - employeeService.addEmployee");
    });
  }

  updateEmployee(){
    var employee = {
      EmployeeId:this.EmployeeId, 
      DepartmentId:this.DepartmentId,
      EmployeeName:this.EmployeeName,
      StartDate:this.StartDate,
      PhotoFileName:this.PhotoFileName,
      Department:null
    };

    this.employeeService.updateEmployee(employee).subscribe(response => {
      console.log(response);
      alert("Success - Employee Updated");
    },
    error => {
      console.log(error);
      alert("Error - Unable to Update Employee - employeeService.updateEmployee");
    });
  }

  uploadPhoto(event:any) {
    var file = event.target.files[0];
    const formData:FormData = new FormData();
    formData.append('uploadedFile', file, file.name);

    this.employeeService.UploadPhoto(formData).subscribe((data:any) => {
      console.log(data);
      this.PhotoFileName = data.toString();
      this.PhotoFilePath = this.employeeService.PhotoUrl + this.PhotoFileName;
    },
    error => {
      console.log(error);
      alert("Error - Unable to retrieve Department Names - getAllDepartmentNames");
    });
  }

}
