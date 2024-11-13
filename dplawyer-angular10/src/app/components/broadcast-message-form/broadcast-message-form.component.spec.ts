import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BroadcastMessageFormComponent } from './broadcast-message-form.component';

describe('BroadcastMessageFormComponent', () => {
  let component: BroadcastMessageFormComponent;
  let fixture: ComponentFixture<BroadcastMessageFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BroadcastMessageFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BroadcastMessageFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
