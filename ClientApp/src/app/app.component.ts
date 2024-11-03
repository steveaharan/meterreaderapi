import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { ReactiveFormsModule } from '@angular/forms';
import { finalize, Subscription } from 'rxjs';
import { HttpClient, HttpEventType } from '@angular/common/http';
import { HttpClientModule } from '@angular/common/http';
import { NgIf } from '@angular/common';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [NgIf, CommonModule, RouterOutlet, MatToolbarModule, MatButtonModule, MatIconModule, MatFormFieldModule, ReactiveFormsModule, MatProgressBarModule, HttpClientModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'ClientApp';
  selectedFile: File | null = null;
  fileName: any;
  uploadProgress:number | undefined;
  uploadSub: Subscription | undefined;
  apiResponse: any;
  successCount: number = 0;
  failedCount: number = 0;
  failedReadings: any[] = [];

  constructor(private http: HttpClient) {}
  
  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input && input.files) {
      this.selectedFile = input.files[0];
      const formData = new FormData();
      formData.append('meterReadingCSV', this.selectedFile);

      const upload$ = this.http.post("http://localhost:5026/meter-reading-uploads", formData, {
        reportProgress: true,
        observe: 'events'
      })
      .pipe(
          finalize(() => this.reset())
      );

      this.uploadSub = upload$.subscribe(event => {
        if (event.type === HttpEventType.UploadProgress) {
          this.uploadProgress = Math.round(100 * (event.loaded / (event.total || 1)));
        } else if (event.type === HttpEventType.Response) {
          console.log('Upload complete ', event.body);
          this.apiResponse = event.body;
          this.successCount = this.apiResponse.successCount;
          this.failedReadings = this.apiResponse.failedReadings;
          this.failedCount = this.apiResponse.failedCount;
        }
      });
    }
  }

  cancelUpload() {
    if (this.uploadSub) {
      this.uploadSub.unsubscribe();
    }
    this.reset();
  }

  reset() {
    this.uploadProgress = undefined;
    this.uploadSub = undefined;
  }

  onUpload() {
    console.log('Uploading file...');
    if (this.selectedFile) {
      const formData = new FormData();
      formData.append('file', this.selectedFile, this.selectedFile.name);

      // Perform the file upload logic here
      console.log('File uploaded:', this.selectedFile);
    }
  }
}
