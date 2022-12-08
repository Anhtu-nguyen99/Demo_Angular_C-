import { Component, Input, OnInit } from '@angular/core';
import { TreeItem } from '@progress/kendo-angular-treeview';
import { Subscription } from 'rxjs';
import { SharedService } from '../share.service';

@Component({
  selector: 'app-Treeview',
  templateUrl: 'treeview.component.html',
  styleUrls: ['treeview.component.css'],
  providers: [ SharedService ]
})
export class TreeviewComponent implements OnInit {

  Node_ID: any;
  //@Input() Application_ID: any;
  @Input() treeviewData: any = [];
  NodeInfo: any = [];
  AttributeInfo: any = [];
  private valueApplication_ID!: Subscription;
  Application_ID :any;
  constructor(private service:SharedService) { }

  showPopup:boolean = false;
  showPopupApp: boolean = false;
  Popup_CreAdd: boolean = false;
  Popup_Search: boolean = false;
  show_Popup_AddNod: boolean = false;
  showPopupAttr: boolean = false;
  isEdit:boolean = false;

  Node_Search: any;

  CollapseAll: boolean = false;

  isNew: boolean = true;
  Node_Parent_ID: any;
  
  Confirm_Remove_Node: boolean = false;

  ParentNodes: any = [];
  public allParentNodes = Array();
  public expandedKeys: any[] = this.ParentNodes.slice();

  

  ngOnInit(): void {
    this.valueApplication_ID = this.service.ValueNodeId.subscribe(id => {this.Application_ID = id;
      this.service.getNodeByApp(this.Application_ID).subscribe(data=> {
        this.treeviewData = data;
      });
    });
    
    console.log(this.Application_ID);
  }

  refresh_Treeview() {
    this.service.getNodeByApp(this.service.ValueAppId.subscribe(id => this.Application_ID = id)).subscribe(data=> {
      this.treeviewData = data;
    });
  }
  //#region Node: search, collapse, expand, remove
  expandNodes() {
    this.service.getAllNodeParent(this.Application_ID).subscribe(data=> {
      this.allParentNodes = data;
      this.allParentNodes.forEach(i => {
        this.ParentNodes.push(i.Node_Title);
      });
      this.expandedKeys = this.ParentNodes.slice();
    });
    
  }

  Collapse() {
    this.expandedKeys = [];
  }

  Popup_Search_Node() {
    this.Popup_Search = !this.Popup_Search;
  }

  Search_Node() {
    this.Popup_Search = !this.Popup_Search;
    this.ParentNodes = Array();
    this.service.searchNode(this.Node_Search).subscribe(data=> {
      if (data.length != 0) {
        this.treeviewData = data;
      }
      else {
        this.refresh_Treeview();
      }
      
      // this.allParentNodes.forEach(i => {
      //   this.ParentNodes.push(i.Node_Title);
      // });
      // console.log(this.allParentNodes);
      // this.expandedKeys = this.ParentNodes.slice();
    });
    
  }

  RemoveNode(status: string) {
    if (status === "yes") {
      if (this.Node_ID != null) {
        var Nod = {Node_ID:this.Node_ID
                  }
        this.service.removeNode(Nod).subscribe(data=>{
          alert(data.toString());
          this.service.getNodeByApp(this.service.ValueAppId.subscribe(id => {this.Application_ID = id})).subscribe(data=> {
            this.treeviewData = data;
          });
        });
      }
      this.Confirm_Remove_Node = false; 
    }

    if (status === 'cancel') {
      this.Confirm_Remove_Node = false;
    }
  }

  Dialog_Remove_Node() {
    this.Confirm_Remove_Node = true;
  }
  //#endregion

  nodeClick(event: TreeItem) :void {
    this.service.getNodeInfo(event.dataItem.Node_ID).subscribe(data=> {
      this.NodeInfo = data;
    });

    this.service.getAttributeList(event.dataItem.Node_ID).subscribe(data=> {
      this.AttributeInfo = data;
    });
    
  }

  iconClass({ Node_Type, Node_Title }: any): any {
    return {
      "k-i-file-pdf": Node_Type == "File",
      "k-i-folder": (Node_Title !== undefined),
      "k-icon": true,
    };
  }

}