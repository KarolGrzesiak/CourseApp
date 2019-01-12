import { Component, OnInit, HostListener } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ValidatorFn, AbstractControl } from '@angular/forms';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { ExamService } from 'src/app/_services/exam.service';
import { ExamForCreation } from 'src/app/_models/exam';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-exams-create',
  templateUrl: './exams-create.component.html',
  styleUrls: ['./exams-create.component.css']
})
export class ExamsCreateComponent implements OnInit {
  examForm: FormGroup;
  exam: ExamForCreation;
  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
    if (this.examForm.dirty) {
      $event.returnValue = true;
    }
  }
  constructor(
    private formBuilder: FormBuilder,
    private examService: ExamService,
    private alertify: AlertifyService,
    private router: Router
  ) {}

  canDeactivate(): Observable<boolean> | Promise<boolean> | boolean {
    if (this.examForm.dirty) {
      return confirm('Do you want to discard the changes ?');
    }
    return true;
  }

  ngOnInit() {
    this.createExamForm();
  }
  createExamForm() {
    this.examForm = this.formBuilder.group(
      {
        name: ['', Validators.required],
        description: [''],
        duration: ['', this.durationValidator()],
        password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
        confirmPassword: ['', Validators.required]
      },
      {
        validator: this.passwordMatchValidator
      }
    );
  }
  passwordMatchValidator(examForm: FormGroup) {
    return examForm.get('password').value === examForm.get('confirmPassword').value
      ? null
      : { mismatch: true };
  }

  durationValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: boolean } | null => {
      if (control.value !== undefined && (isNaN(control.value) || control.value < 0)) {
        return { mismatch: true };
      }
      return null;
    };
  }
  createExam() {
    if (this.examForm.valid) {
      this.exam = Object.assign({}, this.examForm.value);
      this.examService.createExam(this.exam).subscribe(
        () => {
          this.alertify.success('Exam has been created');
          this.examForm.reset();
        },
        error => {
          this.alertify.error(error);
        },
        () => {

          this.router.navigate(['/exams']);
        }
      );
    }
  }
}
