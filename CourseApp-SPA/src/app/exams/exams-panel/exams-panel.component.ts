import { Component, OnInit, ViewChild, HostListener, ElementRef } from '@angular/core';
import { TabsetComponent, BsModalService, BsModalRef } from 'ngx-bootstrap';
import { ExamsEnrolledComponent } from '../exams-enrolled/exams-enrolled.component';
import { ExamsListComponent } from '../exams-list/exams-list.component';
import { AuthService } from 'src/app/_services/auth.service';
import { ExamsDeleteModalComponent } from '../exams-delete-modal/exams-delete-modal.component';
import { Exam } from 'src/app/_models/exam';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ExamService } from 'src/app/_services/exam.service';

@Component({
  selector: 'app-exams-panel',
  templateUrl: './exams-panel.component.html',
  styleUrls: ['./exams-panel.component.css']
})
export class ExamsPanelComponent implements OnInit {
  @ViewChild(ExamsEnrolledComponent) enrolledExams: ExamsEnrolledComponent;
  @ViewChild(ExamsListComponent) examsList: ExamsListComponent;
  bsModalRef: BsModalRef;

  constructor(
    private modalService: BsModalService,
    private authService: AuthService,
    private alertify: AlertifyService,
    private examService: ExamService
  ) {}

  ngOnInit() {}

  enrollOnExam() {
    this.enrolledExams.loadEnrolledExams();
  }

  deleteExamsModal() {
    const initialState = {
      exams: this.getExamsForUser()
    };
    this.bsModalRef = this.modalService.show(ExamsDeleteModalComponent, { initialState });
    this.bsModalRef.content.deleteExams.subscribe((result: Exam[]) => {
      result.forEach(element => {
        this.examService.deleteExam(element.id).subscribe(
          () => {
            const indexInEnrolled = this.enrolledExams.exams.findIndex(e => e.id === element.id);
            if (indexInEnrolled === -1) {
              this.examsList.exams.splice(this.examsList.exams.findIndex(e => e.id === element.id), 1);
            } else {
              this.enrolledExams.exams.splice(
                indexInEnrolled,
                1
              );
            }
          },
          error => {
            this.alertify.error(error);
          }
        );
      });
      this.alertify.success('Exams have been deleted');
    });
  }

  private getExamsForUser() {
    const user = this.authService.currentUser.knownAs;
    return this.examsList.exams
      .filter(e => e.authorKnownAs === user)
      .concat(this.enrolledExams.exams.filter(e => e.authorKnownAs === user));
  }
}
