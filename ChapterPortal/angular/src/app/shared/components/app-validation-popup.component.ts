import { Component, Input, SimpleChange, OnChanges, ChangeDetectorRef } from '@angular/core';

declare var $: any;

@Component({
    selector: '<app-validation-popup>',
    template: `
    <div *ngIf="TempleteValidation.MessageText != ''">
        <span id="errorStatusNlp" [ngClass]="(TempleteValidation.type == 'Error')?'error-msg-nlp':'success-msg-nlp'" class="ErrorMessageNlp">{{TempleteValidation.MessageText}}</span>
    </div>`
})

export class AppValidationPupupComponent {
    @Input() TempleteValidation: any;
    constructor(public _dect: ChangeDetectorRef) { }

    ngOnChanges(changes: { [propKey: string]: SimpleChange }) {
        for (let propName in changes) {
            if (propName == 'TempleteValidation') {
                var height = 55;
                if ($('.modal-dialog:visible').length == 0) {
                    var annTmpl = document.getElementsByClassName('div-announcemet-tmpl');
                    if (annTmpl.length > 0) {
                        var ele: any = annTmpl[0];
                        height += ele.offsetHeight;
                    }
                    var switchTmpl = document.getElementsByClassName('div-switchuserbanner-tmpl');
                    if (switchTmpl.length > 0) {
                        var ele: any = switchTmpl[0];
                        height += ele.offsetHeight;
                    }
                }
                this.TempleteValidation = changes[propName].currentValue;
                this._dect.detectChanges();
                if ($('.modal-dialog:visible').length == 0)
                    $('#errorStatusNlp').css('top',height);
                this.ResponseMessage();
            }

        }
    }

    ResponseMessage() {
        setTimeout(() => {
            //this.TempleteValidation.MessageText = '';
        }, 3000);
    }
}