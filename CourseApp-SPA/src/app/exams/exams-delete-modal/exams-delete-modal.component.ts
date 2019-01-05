import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import { Exam } from 'src/app/_models/exam';

@Component({
  selector: 'app-exams-delete-modal',
  templateUrl: './exams-delete-modal.component.html',
  styleUrls: ['./exams-delete-modal.component.css']
})
export class ExamsDeleteModalComponent implements OnInit {
  @Output() deleteExams = new EventEmitter();

  exams: Exam[];
  constructor(public bsModalRef: BsModalRef) {}

  ngOnInit() {}

  updateExams() {
    const exams = this.exams.filter(e => e.delete === true);
    this.deleteExams.emit(exams);
    this.bsModalRef.hide();
  }
}
