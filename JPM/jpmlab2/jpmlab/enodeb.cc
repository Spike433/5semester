/*
 * enodeb.cc
 *
 *  Created on: Jun 19, 2019
 *      Author: Bart
 */

#include <string.h>
#include <omnetpp.h>

using namespace omnetpp;

class ENodeB : public cSimpleModule {
  protected:
    // The following redefined virtual function holds the algorithm.
    virtual void initialize() override;
    virtual void handleMessage(cMessage *msg) override;
};

// The module class needs to be registered with OMNeT++
Define_Module(ENodeB);

void ENodeB::initialize() {}

void ENodeB::handleMessage(cMessage *msg) {
    if (msg->getKind() == 1){
        send(msg, "out", 1);
        EV << "EnodeB proslijedjuje attach request UE-a za upostavljanjem PDN konekcije MME-u\n";
        EV << "<<<PROSLJEDJIVANJE PAKETA SE NECE VISE ISPISIVATI, OSIM AKO JE BITNO, NO LOGIKA PROSLJEDJIVANJA JE I DALJE IMPLEMENTIRANA>>>\n";
    }
    if (msg->getKind() == 2){
        if (msg->arrivedOn("in", 1)){
            send(msg, "out", 0);
        }else{
            send(msg, "out", 1);
        }
    }
    if (msg->getKind() == 5){
        cMessage (*rbconf) = new cMessage("RadioBearerConfig", 6);
        send(rbconf, "out", 0);
        EV << "Rekonfiguracija radio bearer-a na temelju parametara primljenih od MME-a (eNodeB)\n";
    }
    if (msg->getKind() == 6){
        cMessage (*iniContext) = new cMessage("InitialContextResponse", 7);
        send(iniContext, "out", 1);
        EV << "eNodeB salje Initial context odgovor MME-u\n";
    }
    if (msg->getKind() == 8){
        cMessage (*attachComplete) = new cMessage("AttachCompleteMessage", 9);
        send(attachComplete, "out", 1);
        EV << "eNodeB proslijedjuje attach complete poruku MME-u\n";
    }
    if (msg->getKind() == 10){
        send(msg, "out", 2);
    }
    if (msg->getKind() == 13){
        send(msg, "out", 0);
    }
}


