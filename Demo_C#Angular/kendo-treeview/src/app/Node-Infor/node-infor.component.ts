import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, NgForm, Validators } from '@angular/forms';
import { AddEvent, CancelEvent, EditEvent, GridComponent, RemoveEvent, SaveEvent } from '@progress/kendo-angular-grid';
import { AppComponent } from '../app.component';
import { SharedService } from '../share.service';

@Component({
  selector: 'app-NodeInfor',
  templateUrl: './Node-Infor.component.html',
  styleUrls: ['./Node-Infor.component.css'],
  providers: [ SharedService ]
})
export class NodeInforComponent implements OnInit {

  editedRowIndex: number | any;
  formGroup: FormGroup | any;
  constructor(private service:SharedService) { }

  ngOnInit(): void {
    this.service.getNodeInfo(this.service.ValueNodeId.subscribe(id => {this.NodeInfomation.Node_ID = id})).subscribe(data=> {
      this.NodeInfomation = data;
    });
  }
  @Input() AttributeInfo: any = [];
  @Input() NodeInfomation: any = [];
  
  Confirm_Save: boolean = false;
  showPopup: boolean = false;
  selectedNode: any;

  isNew: boolean = true;

  UpdateNode(status: string ) {
    if (status === 'yes') {
      var node = {Node_Title:this.NodeInfomation.Node_Title,
                  Node_Type:this.NodeInfomation.Node_Type,
                  Owner:'Admin',
                  Node_ID:this.NodeInfomation.Node_ID,
                  Application_ID:this.NodeInfomation.Application_ID
                }
      this.service.updateNode(node).subscribe(res=>{
        alert(res.toString())
        this.service.getNodeInfo(this.service.ValueNodeId.subscribe(id => {this.NodeInfomation.Node_ID = id})).subscribe(data=> {
          this.NodeInfomation = data;
        });
        this.service.InputValueAppId(this.NodeInfomation.Application_ID);
      });
      
      this.Confirm_Save = false;
    }

    if (status === 'cancel') {
      this.Confirm_Save = false;
    }
  }

  Dialog_Save() {
    this.Confirm_Save = true;
  }

  PopUp_Edit_Detail(): void {
    this.showPopup = !this.showPopup;
  }

  public addHandler(args: AddEvent, formInstance: NgForm): void {
    formInstance.reset();
    this.closeEditor(args.sender);
    this.isNew = true;

      this.formGroup = new FormGroup({
        Attribute_Name: new FormControl("", Validators.required),
        Node_ID: new FormControl(),
      });

    args.sender.addRow(this.formGroup);
  }

  public editHandler(args: EditEvent): void {
    this.closeEditor(args.sender);
    this.isNew = false;

    this.editedRowIndex = args.rowIndex;
    args.sender.editRow(args.rowIndex);
  }

  public cancelHandler(args: CancelEvent): void {
    this.closeEditor(args.sender, args.rowIndex);
  }

  public saveHandler({ sender, rowIndex, dataItem}: SaveEvent): void {
    sender.closeRow(rowIndex);
    //add attribute
    if (this.isNew == true) {
      dataItem.Node_ID = this.NodeInfomation.Node_ID;
      this.service.addAttribute(dataItem).subscribe(res=>{
        alert(res.toString())
        this.service.getAttributeList(this.NodeInfomation.Node_ID).subscribe(data=> {
          this.AttributeInfo = data;
        });
      });
    }
    //update attribute
    if (this.isNew == false) {
      this.service.Update_Attr(dataItem).subscribe(res=>{
        alert(res.toString())
      });
    }
  }

  public removeHandler(args: RemoveEvent): void {
    this.service.removeAttribute(args.dataItem).subscribe(data=>{
      alert(data.toString());
      this.service.getAttributeList(this.NodeInfomation.Node_ID).subscribe(data=> {
        this.AttributeInfo = data;
      });
    });
  }

  private closeEditor(grid: GridComponent, rowIndex = this.editedRowIndex) {
    grid.closeRow(rowIndex);

    this.editedRowIndex = undefined;
    this.formGroup = undefined;
  }
}