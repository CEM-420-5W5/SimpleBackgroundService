import { Component } from '@angular/core';

// On doit commencer par ajouter signalr dans les node_modules: npm install @microsoft/signalr
// Ensuite on inclut la librairie
import * as signalR from "@microsoft/signalr"

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'ngBackgroundService';

  baseUrl = "http://localhost:5080/";

  private hubConnection?: signalR.HubConnection

  isConnected = false;
  delayInSeconds = 5;
  texte = "";
  
  ChangeDelay() {
    this.hubConnection!.invoke('ChangeDelay', this.delayInSeconds);
  }

  AddMessage() {
    this.hubConnection!.invoke('AddMessage', this.texte);
    this.texte = "";
  }

  ClearAllMessages() {
    this.hubConnection!.invoke('ClearAllMessages');
  }

  connectToHub() {
    this.hubConnection = new signalR.HubConnectionBuilder()
                              .withUrl(this.baseUrl + 'spammer')
                              .build();

    this.hubConnection.on('Spam', (messages:string[]) => {
      console.log("SPAMMING!")
      console.log(messages);
    });

    this.hubConnection
      .start()
      .then(() => {
        console.log("Connecté au Hub");
        this.isConnected = true;
      })
      .catch(err => console.log('Error while starting connection: ' + err))
  }
}
