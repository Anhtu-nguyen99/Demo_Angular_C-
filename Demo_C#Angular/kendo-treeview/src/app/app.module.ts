import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { TreeviewComponent } from './Treeview/treeview.component';
import { NodeInforComponent } from './Node-Infor/node-infor.component';
import { HttpClientModule } from '@angular/common/http';
import { SharedService } from './share.service';
import { TreeViewModule } from "@progress/kendo-angular-treeview";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { GridModule } from '@progress/kendo-angular-grid';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { ButtonsModule } from "@progress/kendo-angular-buttons";
import { InputsModule } from "@progress/kendo-angular-inputs";
import { FormsModule } from '@angular/forms';
import { PopupModule } from '@progress/kendo-angular-popup';
import { MatTabsModule } from '@angular/material/tabs';
import { DialogsModule } from '@progress/kendo-angular-dialog';
import { ToolBarModule } from '@progress/kendo-angular-toolbar';
import { DropDownsModule } from '@progress/kendo-angular-dropdowns';

@NgModule({
  declarations: [
    AppComponent,
    TreeviewComponent,
    NodeInforComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    TreeViewModule,
    BrowserAnimationsModule,
    GridModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    ButtonsModule,
    InputsModule,
    FormsModule,
    PopupModule,
    MatTabsModule,
    DialogsModule,
    ToolBarModule,
    DropDownsModule
  ],
  providers: [SharedService],
  bootstrap: [AppComponent]
})
export class AppModule { }
