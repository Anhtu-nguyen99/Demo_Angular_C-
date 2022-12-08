import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';

@Injectable()
  export class SharedService {
  readonly APIUrl = "https://localhost:5001/api";
  
  constructor(private http:HttpClient) {}

  private Application_ID :Subject<number> = new Subject();
  
  private SubNode_ID :Subject<number> = new Subject();

  //#region update

  updateNode(val:any) {
    return this.http.put(this.APIUrl + '/Node', val);
  }
  //#endregion

  //#region getBy

  getDepartmentByCompany(val:any):Observable<any>{
    return this.http.get(this.APIUrl + '/Department/getBy/' + val);
  }

  getProjectByDepartment(val:any):Observable<any>{
    return this.http.get(this.APIUrl + '/Project/getBy/' + val);
  }

  getEmployeeByProject(val:any):Observable<any>{
    return this.http.get(this.APIUrl + '/Employee/getBy/' + val);
  }

  getTaskByEmployee(val:any):Observable<any>{
    return this.http.get(this.APIUrl + '/Task/getBy/' + val);
  }

  //#endregion

  //#region get node and attribute

  get ValueAppId() {
    return this.Application_ID;
  }
  InputValueAppId(id: number) {
    this.Application_ID.next(id);
  }
  getNodeByApp(val:any):Observable<any> {
    return this.http.get(this.APIUrl + '/Node/getNodeByApp/' + val);
  }

  getDataAllNode():Observable<any>{
    return this.http.get(this.APIUrl + '/Node');
  }

  getDataApplication():Observable<any>{
    return this.http.get(this.APIUrl + '/Application');
  }

  getAllNodeParent(val:any):Observable<any>{
    return this.http.get(this.APIUrl + '/Node/getNodeParent/' + val);
  }

  get ValueNodeId() {
    return this.SubNode_ID;
  }
  InputValueNodeInfo(id: any) {
    this.SubNode_ID.next(id);
  }
  getNodeInfo(val:any):Observable<any> {
    return this.http.get(this.APIUrl + '/Node/OneNode/' + val);
  }
  

  getAttributeList(val:any):Observable<any>{
    return this.http.get(this.APIUrl + '/Attribute/GetByNode/' + val);
  }

  getAllAttribute():Observable<any>{
    return this.http.get(this.APIUrl + '/Attribute');
  }

  searchNode(val:any):Observable<any>{
    return this.http.get(this.APIUrl + '/Node/searchNode/' + val);
  }


  //#endregion

  //#region Remove

  removeAttribute(val:any) {
    return this.http.put(this.APIUrl + '/Attribute/Delete', val);
  }

  removeApplication(val:any) {
    return this.http.put(this.APIUrl + '/Application/Delete', val);
  }

  removeNode(val:any) {
    return this.http.put(this.APIUrl + '/Node/Delete', val);
  }

  //#endregion

  //#region insert attributes

  addAttribute(val:any) {
    return this.http.post(this.APIUrl + '/Attribute', val);
  }

  addApplication(val:any) {
    return this.http.post(this.APIUrl + '/Application', val);
  }

  addNode(val:any) {
    return this.http.post(this.APIUrl + '/Node', val);
  }

  //#endregion

  //#region updateAttribute 

  Update_Attr(val:any) {
    return this.http.put(this.APIUrl + '/Attribute/UpdateAttribute', val);
  }

  Update_App(val:any) {
    return this.http.put(this.APIUrl + '/Application', val);
  }
  
  //#endregion
}