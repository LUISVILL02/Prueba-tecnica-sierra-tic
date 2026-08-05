import { TestBed } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';

import { TodoManagement } from './todo-management';

describe('TodoManagement', () => {
  let service: TodoManagement;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideHttpClientTesting()],
    });
    service = TestBed.inject(TodoManagement);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
