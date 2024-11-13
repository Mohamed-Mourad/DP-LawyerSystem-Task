import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserMessageFormComponent } from './user-message-form.component';

describe('UserMessageFormComponent', () => {
  let component: UserMessageFormComponent;
  let fixture: ComponentFixture<UserMessageFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserMessageFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserMessageFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
