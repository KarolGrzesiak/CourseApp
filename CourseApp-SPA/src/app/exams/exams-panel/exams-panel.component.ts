import { Component, OnInit, ViewChild } from '@angular/core';
import { TabsetComponent } from 'ngx-bootstrap';
import { ExamsEnrolledComponent } from '../exams-enrolled/exams-enrolled.component';

@Component({
  selector: 'app-exams-panel',
  templateUrl: './exams-panel.component.html',
  styleUrls: ['./exams-panel.component.css']
})
export class ExamsPanelComponent implements OnInit {
  @ViewChild(ExamsEnrolledComponent) enrolledExams: ExamsEnrolledComponent;
  constructor() {}

  ngOnInit() {}
  onSelectEnrolledExams() {
    this.enrolledExams.loadEnrolledExams();
  }
}
