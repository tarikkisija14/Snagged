import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-button',
  templateUrl: './button.component.html',
  standalone:false,
  styleUrls: ['./button.component.scss']
})
export class ButtonComponent {
  @Input() label: string = 'Click me';
  @Input() type: 'button' | 'submit' | 'reset' = 'button';
  @Input() disabled: boolean = false;
  @Input() variant: 'primary' | 'primary-sell' | 'secondary' = 'primary';
  @Input() className: string = '';

  @Output() onClick = new EventEmitter<Event>();

  handleClick(event: Event) {
    if (!this.disabled) {
      this.onClick.emit(event);
    }
  }
}
