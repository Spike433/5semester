/*
 * sgw.cc
 *
 *  Created on: Jun 19, 2019
 *      Author: Bart
 */

#include <string.h>
#include <omnetpp.h>

using namespace omnetpp;

class SGW : public cSimpleModule {
protected:
    // The following redefined virtual function holds the algorithm.
    virtual void initialize() override;
    virtual void handleMessage(cMessage *msg) override;
};

// The module class needs to be registered with OMNeT++
Define_Module(SGW);

void SGW::initialize() {}

void SGW::handleMessage(cMessage *msg) {
    if (msg->getKind() == 3){
        send(msg, "out", 3);
        EV << "S-GW proslijedjuje create session request nadleznom P-GW-u\n";
    }
    if (msg->getKind() == 4){
        send(msg, "out", 1);
        EV << "S-GW proslijedjuje create session response MME-u otkud je request dosao\n";
    }
    if (msg->getKind() == 11){
        cMessage *modifyBearer = new cMessage("ModifyBearerResponseMessage", 11);
        send(modifyBearer, "out", 1);
        EV << "S-GW odgovara sa modify bearer porukom.\n UE sada moze i primati pakete\n";
    }
    if (msg->getKind() == 10){
        send(msg, "out", 3);
    }
    if (msg->getKind() == 13){
        send(msg, "out", 0);
    }
}
