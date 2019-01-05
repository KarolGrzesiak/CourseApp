import { Question } from './question';
import { User } from './user';

export interface Exam {
  id: number;
  authorKnownAs: string;
  name: string;
  description: string;
  datePublished: Date;
  duration?: string;
  questions?: Question[];
  delete: boolean;
}
