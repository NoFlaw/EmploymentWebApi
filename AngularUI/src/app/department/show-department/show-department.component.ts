import { Component, OnInit} from '@angular/core';
import { DepartmentService } from 'src/app/department.service';
import * as $ from "jquery"; 
import { Subject } from 'rxjs';

@Component({
  selector: 'app-show-department',
  templateUrl: './show-department.component.html',
  styleUrls: ['./show-department.component.css']
})
export class ShowDepartmentComponent implements OnInit {
  Department:any;
  DepartmentList:any=[];

  ModalTitle:string = "";
  ActivateAddEditDepartmentComponent:boolean = false;

  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<any> = new Subject<any>();

  constructor(private departmentService: DepartmentService) { }

  ngOnInit(): void {
    
    this.dtOptions = {
      pagingType: 'full_numbers',
      pageLength: 10
    };

    this.Department = {
      DepartmentId: 0,
      DepartmentName: "",
      IsActive:false
    };

    this.refreshDepartmentList();

  }

  addClick(){
    this.Department = {
      DepartmentId: 0,
      DepartmentName: "",
      IsActive:true
    }

    this.ModalTitle = "Add Department";
    this.ActivateAddEditDepartmentComponent = true;
  }

  editClick(item: any) {  
    this.Department = item;    
    this.ModalTitle = "Edit Department"
    this.ActivateAddEditDepartmentComponent = true;
  }

  deleteClick(item: any) {
    if (confirm('Are you sure you want to remove this Department?')){
      this.departmentService.deleteDepartment(item.DepartmentId).subscribe(response => {
        console.log(response);
        alert("Success - Department Removed");
        this.refreshDepartmentList();
      },
      error => {
        console.log(error);
        alert("Error - Unable to Remove Department");
      });
    }
  } 

  closeClick() {
    this.dtTrigger.unsubscribe();
    this.ActivateAddEditDepartmentComponent = false;
    this.refreshDepartmentList();
  }

  refreshDepartmentList(){
    this.departmentService.getDepartmentList().subscribe(data => {
      this.DepartmentList = data;
      this.dtTrigger.next();
    });
  }

  ngOnDestroy(): void {
    // Do not forget to unsubscribe the event
    this.dtTrigger.unsubscribe();
  }

}
