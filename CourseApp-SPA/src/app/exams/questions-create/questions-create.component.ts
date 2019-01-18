import { Component, OnInit, HostListener } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators, FormArray, FormControl } from '@angular/forms';
import { ExamService } from 'src/app/_services/exam.service';
import { Answer } from 'src/app/_models/answer';
import { Exam } from 'src/app/_models/exam';
import { Question } from 'src/app/_models/question';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-questions-create',
  templateUrl: './questions-create.component.html',
  styleUrls: ['./questions-create.component.css']
})
export class QuestionsCreateComponent implements OnInit {
  examId: number;
  questionForm: FormGroup;
  count: number;
  questions: Question[];
  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
    if (this.questionForm.dirty) {
      $event.returnValue = true;
    }
  }
  constructor(
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    private examService: ExamService,
    private alertify: AlertifyService,
    private router: Router
  ) {}

  canDeactivate(): Observable<boolean> | Promise<boolean> | boolean {
    if (this.questionForm.dirty) {
      return confirm('Do you want to discard the changes ?');
    }
    return true;
  }

  ngOnInit() {
    this.count = 1;
    this.examId = +this.route.snapshot.paramMap.get('examId');
    this.createQuestionForm();
  }
  createQuestionForm() {
    this.questionForm = this.formBuilder.group({
      type: ['Text'],
      questionContent: ['', Validators.required],
      answers: this.formBuilder.array([this.initAnswers('true')], Validators.required)
    });
  }

  initAnswers(isCorrect: string, content?: string) {
    return this.formBuilder.group({
      answerContent: [content, Validators.required],
      isCorrect: [isCorrect]
    });
  }
  chooseCorrectAnswer(number: number) {
    const control = <FormArray>this.questionForm.controls['answers'];
    const correctAnswer = control.controls[number].get('isCorrect');
    if (correctAnswer.value === 'true') {
      return;
    }
    for (let i = 0; i < control.length; i++) {
      control.controls[i].get('isCorrect').setValue('false');
    }
    correctAnswer.setValue('true');
  }
  addAnswer() {
    // add address to the list
    const control = <FormArray>this.questionForm.controls['answers'];
    control.push(this.initAnswers('false'));
  }

  createAnswers() {
    const answers = this.questionForm.get('answers').value;
    if (answers.length > 1) {
      const control = <FormArray>this.questionForm.controls['answers'];
      const content = control.controls[0].get('answerContent').value;
      this.questionForm.setControl(
        'answers',
        this.formBuilder.array([this.initAnswers('true', content)])
      );
    } else {
      for (let i = 0; i < 3; i++) {
        this.addAnswer();
      }
    }
  }

  removeAnswer(i: number) {
    // remove address from the list
    const control = <FormArray>this.questionForm.controls['answers'];
    control.removeAt(i);
  }

  get answers(): FormArray {
    return this.questionForm.get('answers') as FormArray;
  }

  createQuestion() {
    const control = <FormArray>this.questionForm.controls['answers'];
    console.log(control.controls[0].get('answerContent').value);
  }

  // deleteExam() {
  //   this.examService.deleteExam(this.examId).subscribe(
  //     () => {
  //       this.alertify.error('Exam has been cancelled');
  //     },
  //     error => {
  //       this.alertify.error(error);
  //     }
  //   );
  // }
  finish() {
    if (this.questionForm.valid) {
      const questionForCreation = {
        content: this.questionForm.get('questionContent').value,
        type: this.questionForm.get('type').value
      };
      this.examService.createQuestion(questionForCreation, this.examId).subscribe(
        (question: Question) => {
          const answers = [];
          const control = <FormArray>this.questionForm.controls['answers'];
          for (let i = 0; i < control.length; i++) {
            answers.push({
              content: control.controls[i].get('answerContent').value,
              isCorrect: control.controls[i].get('isCorrect').value
            });
          }
          this.examService.createAnswers(answers, question.id).subscribe(
            () => {
              this.questionForm.reset();
              this.alertify.success('Exam completed');
              this.router.navigate(['/exams']);
            },
            error => {
              this.alertify.error(error);
            }
          );
        },
        error => {
          this.alertify.error(error);
        }
      );
    }
  }
  cancel() {
    if (this.questionForm.dirty) {
      this.alertify.confirm('Are you sure?', () => {
        this.questionForm.reset();
        this.router.navigate(['/exams']);
      });
    } else {
      this.questionForm.reset();
      this.router.navigate(['/exams']);
    }
  }
  nextQuestion() {
    if (this.questionForm.valid) {
      const questionForCreation = {
        content: this.questionForm.get('questionContent').value,
        type: this.questionForm.get('type').value
      };
      this.examService.createQuestion(questionForCreation, this.examId).subscribe(
        (question: Question) => {
          const answers = [];
          const control = <FormArray>this.questionForm.controls['answers'];
          for (let i = 0; i < control.length; i++) {
            answers.push({
              content: control.controls[i].get('answerContent').value,
              isCorrect: control.controls[i].get('isCorrect').value
            });
          }
          this.examService.createAnswers(answers, question.id).subscribe(
            () => {
              this.questionForm.reset();
              this.createQuestionForm();
              this.alertify.success(this.count + ' Question created');
              this.count++;
            },
            error => {
              this.alertify.error(error);
            }
          );
        },
        error => {
          this.alertify.error(error);
        }
      );
    }
  }
}
