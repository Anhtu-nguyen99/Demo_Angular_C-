import { Component, OnInit, Input } from '@angular/core';
import { SharedService } from "src/app/share.service";
import { FormGroup, FormControl, Validators, NgForm } from "@angular/forms";
import {
  GridComponent,
  CancelEvent,
  EditEvent,
  RemoveEvent,
  SaveEvent,
  AddEvent,
} from "@progress/kendo-angular-grid";
import { Offset } from '@progress/kendo-angular-popup';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{

  formGroup: FormGroup | any;
  editedRowIndex: number | any;

  constructor(private service:SharedService ) {}

  title = 'kendo-treeview';
  
  @Input() Application_Name: any;
  treeviewData: any = [];
  AppData: any = [];
  NodeData: any = [];
  AttributeData: any = [];
  test = '&nbsp<abc>';
  selectedApp: any;
  selectedNodeParents: any;
  selectedNode: any;
  
  Node_ID: any;
  Application_ID: any;

  addHandler_Active:boolean = false; //false: active update app, true: active update node

  
  showPopupApp: boolean = false;
  Popup_CreAdd: boolean = false;
  show_Popup_AddNod: boolean = false;
  showPopupAttr: boolean = false;
  isEdit:boolean = false;

  isNew: boolean = true;

  offset_App: Offset = {left:600, top:200};

  ngOnInit(): void {
    this.service.ValueNodeId.subscribe(id => {this.Application_ID = id});
    this.LoadAllData();
    
  }

  LoadAllData() {
    
    this.service.getDataApplication().subscribe(data=> {
      this.AppData = data;
    });
    this.service.getDataAllNode().subscribe(data=> {
      this.NodeData = data;
      console.log(this.NodeData);
    });
    this.service.getAllAttribute().subscribe(data=> {
      this.AttributeData = data;
    });
  }

  TreeAppData() {
    // this.service.getNodeByApp(this.Application_ID).subscribe(data=> {
    //   this.treeviewData = data;
    // });
    this.service.ValueNodeId.subscribe(id => {this.Application_ID = id});
    console.log(this.Application_ID);
    this.showPopupApp = !this.showPopupApp;
  }

  //#region Popup & icon

  PopUpApp() {
    this.showPopupApp = !this.showPopupApp;
    this.addHandler_Active == false;
  }

  Popup_CreApp() {
    this.Popup_CreAdd = !this.Popup_CreAdd;
  }

  Create_App() {
    var App = {Application_Name:this.Application_Name,
                Owner:'Admin',
              }
    this.service.addApplication(App).subscribe(res=>{
    alert(res.toString())
    this.LoadAllData();
    });
    this.Popup_CreAdd = !this.Popup_CreAdd;
  }

  PopUp_AddNode() {
    this.addHandler_Active = true;
    this.show_Popup_AddNod = !this.show_Popup_AddNod;
  }

  PopUp_Attr() {
    this.showPopupAttr = !this.showPopupAttr;
    this.addHandler_Active = false;
  }
  //#endregion

  //#region function grid handler attribute

  public addHandler(args: AddEvent, formInstance: NgForm): void {
    formInstance.reset();
    this.closeEditor(args.sender);
    this.isNew = true;
    this.isEdit = true;   

    this.formGroup = new FormGroup({
      Node_Title: new FormControl("", Validators.required),
      Node_Type: new FormControl("", Validators.required),
      Owner: new FormControl("", Validators.required),
      Application_ID: new FormControl(),
      Node_Parent_ID: new FormControl(),
    });
    
    args.sender.addRow(this.formGroup);
  }

  public editHandler(args: EditEvent): void {
    this.closeEditor(args.sender);
    this.isNew = false;
    this.isEdit = false;
    this.editedRowIndex = args.rowIndex;
    args.sender.editRow(args.rowIndex);
  }

  public cancelHandler(args: CancelEvent): void {
    this.closeEditor(args.sender, args.rowIndex);
  }

  public saveHandler({ sender, rowIndex, dataItem}: SaveEvent): void {
    sender.closeRow(rowIndex);

    if (this.isNew == true) {
      dataItem.Application_ID = this.selectedApp;
      dataItem.Node_Parent_ID = this.selectedNodeParents;
      this.service.addNode(dataItem).subscribe(res=>{
        alert(res.toString())
        this.LoadAllData();
      });
    }

    if (this.isNew == false) {
      if ((this.addHandler_Active == false) && (dataItem.Application_ID != null)) {
        this.service.Update_App(dataItem).subscribe(res=>{
          alert(res.toString())
          this.LoadAllData();
        });
      }

      if (this.addHandler_Active == true) {
        dataItem.Application_ID = this.selectedApp;
        this.service.updateNode(dataItem).subscribe(res=>{
          alert(res.toString())
          this.LoadAllData();
        });
      }

      if (dataItem.Attribute_ID) {
        dataItem.Node_ID = this.selectedNode;
        this.service.Update_Attr(dataItem).subscribe(res=>{
          alert(res.toString())
          this.LoadAllData();
        });
      }
    }
  }

  public removeHandler(args: RemoveEvent): void {
    if (args.dataItem.Application_ID) {
      this.service.removeApplication(args.dataItem).subscribe(data=>{
        alert(data.toString());
        this.LoadAllData();
      });
    }
  }

  private closeEditor(grid: GridComponent, rowIndex = this.editedRowIndex) {
    grid.closeRow(rowIndex);

    this.editedRowIndex = undefined;
    this.formGroup = undefined;
  }
  //#endregion

  selectedRow(event: any) {
    this.service.InputValueAppId(event.dataItem.Application_ID);
    console.log(this.Application_ID);
  }

}
