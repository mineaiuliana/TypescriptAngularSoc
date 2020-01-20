import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-confirmation-modal',
  templateUrl: './confirmation-modal.component.html',
  styleUrls: ['./confirmation-modal.component.scss']
})

export class ConfirmationModalComponent {
  @Input() isVisible: boolean;
  @Input() from: string;
  @Input() to: string;
  @Output() closeModal = new EventEmitter<boolean>();

  constructor() { }

  onConfirmation() {
    this.closeModal.emit(true);
  }
  onCancel() {
    this.closeModal.emit(false);
  }
}
