import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-rock-paper-scissors',
  templateUrl: './rock-paper-scissors.component.html',
  styleUrls: ['./rock-paper-scissors.component.css']
})
export class RockPaperScissorsComponent implements OnInit {
  scores = [0, 0];
  weapons = ['rock', 'paper', 'scissors'];
  playerSelected = -1;
  loading = false;
  isResultShow = false;


  theResult = 0;
  enemySelected = -1;

  pick(weapon: number): void {

    if (this.loading) {
      return;
    }
    this.loading = true;
    this.playerSelected = weapon;

    setTimeout(() => {
      this.loading = false;
      const randomNum = Math.floor(Math.random() * 3);
      this.enemySelected = randomNum;
      this.checkResult();
      this.isResultShow = true;
    }, Math.floor(Math.random() * 500) + 200);
  }

  reset(): void {
    this.scores = [0, 0];
  }
  checkResult(): void {
    const playerPick = this.playerSelected;
    const enemyPick = this.enemySelected;
    if (playerPick === enemyPick) {
      this.theResult = 2;
    } else if ((playerPick - enemyPick + 3) % 3 === 1) {
      this.theResult = 0;
      this.scores[0] = this.scores[0] + 1;
    } else {
      this.theResult = 1;
      this.scores[1] = this.scores[1] + 1;
    }
  }

  ngOnInit() {}
}
