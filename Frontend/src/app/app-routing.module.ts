import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { HomeComponent } from './home/home.component';
import { UsersComponent } from './users/users.component';
import { UsersStudentComponent } from './users-student/users-student.component';
import { InventoryComponent } from './inventory/inventory.component';
import { ProfileComponent } from './profile/profile.component';
import { LoanComponent } from './loan/loan.component';
import { CreateUserComponent } from './create-user/create-user.component';
import { AddressComponent } from './address/address.component';
import { BuildingComponent } from './buildings/buildings.component';
import { RoomComponent } from './room/room.component';
import { AuthGuard } from './guards/auth.guard';
import { RoleGuard } from './guards/role.guard';
import { ItemDetailsComponent } from './itemDetails/itemDetails.component';
import { ItemGroupsComponent } from './itemgroups/itemgroups.component';
import { RolesComponent } from './roles/roles.component';
import { StatusComponent } from './status/status.component';
import { ItemtypeComponent } from './itemtype/itemtype.component';
import { RequestComponent } from './request/request.component';
import { StatusHistoryComponent } from './status-history/status-history.component';
import { ArchiveMenuComponent } from './archive/archive-menu/archive-menu.component';
import { ItemArchiveComponent } from './archive/item-archive/item-archive.component';
import { ItemgroupArchiveComponent } from './archive/itemgroup-archive/itemgroup-archive.component';
import { LoanArchiveComponent } from './archive/loan-archive/loan-archive.component';
import { RequestArchiveComponent } from './archive/request-archive/request-archive.component';
import { UserArchiveComponent } from './archive/user-archive/user-archive.component';
import { ItemtypeArchiveComponent } from './archive/itemtype-archive/itemtype-archive.component';
import { ComputerComponent } from './computer/computer.component';
import { ComputerpartComponent } from './computerpart/computerpart.component';
import { PartGroupsComponent } from './part-groups/part-groups.component';
import { PartTypeComponent } from './part-type/part-type.component';

export const routes: Routes = [
  //Default route
  { path: '', redirectTo: 'login', pathMatch: 'full' },

  //No Auth Guard needed or Role Guard needed for login
  { path: 'login', component: LoginComponent },

  //No Auth Guard needed
  { path: 'home', component: HomeComponent, canActivate: [AuthGuard] },
  { path: 'profile', component: ProfileComponent, canActivate: [AuthGuard] },

  //For User navbar
  {
    path: 'users',
    component: UsersComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: [1] },
  },
  {
    path: 'students',
    component: UsersStudentComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: [1, 2, 4] },
  },
  {
    path: 'roles',
    component: RolesComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: [1] },
  },

  //For Inventory Navbar
  {
    path: 'itemgroups',
    component: ItemGroupsComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: [1, 4] },
  },
  {
    path: 'itemtype',
    component: ItemtypeComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: [1, 4] },
  },
  {
    path: 'inventory',
    component: InventoryComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: [1, 4] },
  },
  {
    path: 'itemDetails/:id',
    component: ItemDetailsComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: [1, 2, 4] },
  },
  {
    path: 'status',
    component: StatusComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: [1] },
  },
  {
    path: 'statushistory',
    component: StatusHistoryComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: [1, 4] },
  },

  // For Computer Navbar
  { path: 'computer', component: ComputerComponent, canActivate: [AuthGuard, RoleGuard], data: { roles: [1, 2] } },
  { path: 'computerPart', component: ComputerpartComponent, canActivate: [AuthGuard, RoleGuard], data: { roles: [1, 2] } },
  { path: 'partGroups', component: PartGroupsComponent, canActivate: [AuthGuard, RoleGuard], data: { roles: [1, 2] } },
  { path: 'part-type', component: PartTypeComponent, canActivate: [AuthGuard, RoleGuard], data: { roles: [1] } },

  // For Location Navbar
  {
    path: 'address',
    component: AddressComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: [1, 4] },
  },
  {
    path: 'buildings',
    component: BuildingComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: [1, 4] },
  },
  {
    path: 'room',
    component: RoomComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: [1, 4] },
  },

  //For Loan Navbar
  {
    path: 'request',
    component: RequestComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: [1, 2, 4] },
  },

  {
    path: 'loan',
    component: LoanComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: [1, 2, 4] },
  },

  // Used in brugere page and elever page
  {
    path: 'create-user',
    component: CreateUserComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: [1, 2, 4] },
  },

  // Archive
  {
    path: 'archive-menu',
    component: ArchiveMenuComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: [1] },
  },
  {
    path: 'item-archive',
    component: ItemArchiveComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: [1] },
  },
  {
    path: 'itemgroup-archive',
    component: ItemgroupArchiveComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: [1] },
  },
  {
    path: 'loan-archive',
    component: LoanArchiveComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: [1] },
  },
  {
    path: 'request-archive',
    component: RequestArchiveComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: [1] },
  },
  {
    path: 'user-archive',
    component: UserArchiveComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: [1] },
  },
  {
    path: 'itemtype-archive',
    component: ItemtypeArchiveComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: [1] },
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }
