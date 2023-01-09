/*
 * ue.cc
 *
 *  Created on: Jun 19, 2019
 *      Author: Bart
 */

#include <string.h>
#include <omnetpp.h>


using namespace omnetpp;

class UE : public cSimpleModule {
  protected:
    // The following redefined virtual function holds the algorithm.
    virtual void initialize() override;
    virtual void handleMessage(cMessage *msg) override;
};

// The module class needs to be registered with OMNeT++
Define_Module(UE);

void UE::initialize() {
    cMessage (*reqDatConn) = new cMessage("RequestDataConnection", 1);
    send(reqDatConn, "out");
    EV << "UE zeli uspostaviti PDN konekciju sa PDN-GW, te salje attach request EnodeB-u\n";
    EV << getFullPath();
}

void UE::handleMessage(cMessage *msg) {
    if (msg->getKind() == 2){
        cMessage (*authRes) = new cMessage("AuthenticationResponse", 2);
        send(authRes, "out");
        EV << "UE salje Auth. response MME-u u kojem se nalazi RES odgovor\n";
    }
    if (msg->getKind() == 6){
        send(msg, "out");
        EV << "Rekonfiguracija radio bearer-a na temelju parametara primljenih od MME-a (UE)\n";

        cMessage (*directTransfer) = new cMessage("DirectTransferMessage", 8);
        sendDelayed(directTransfer, SimTime(2, SIMTIME_S), "out");

        cMessage (*ok) = new cMessage("ok", 20);
        scheduleAt(simTime() + SimTime(2, SIMTIME_S), ok);

        cMessage (*upload) = new cMessage("UploadPacket", 10);
        sendDelayed(upload, SimTime(4, SIMTIME_S), "out");

        cMessage (*ok2) = new cMessage("ok2", 21);
        scheduleAt(simTime() + SimTime(4, SIMTIME_S), ok2);

    }
    if (msg->getKind() == 13){
        cMessage (*upload) = new cMessage("UploadPacket", 10);
        send(upload, "out");
    }
    if (msg->getKind() == 20){
        EV << "UE salje direct transfer poruku koja u sebi sadrzi attach complete signal\n";
    }
    if (msg->getKind() == 21){
        EV << "UE moze slati uplink pakete\n";
    }


}

