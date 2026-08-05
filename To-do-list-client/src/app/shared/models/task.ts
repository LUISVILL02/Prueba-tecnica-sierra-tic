export interface CreateTaskInput {
  title: string;
  description: string;
}

export interface UpdateTaskInput {
  id: number;
  payload: CreateTaskInput;
}

export interface Task {
  id: number;
  title: string;
  description: string;
  isCompleted: boolean;
  createdAt: string;
}