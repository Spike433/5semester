/*
 * hss.cc
 *
 *  Created on: Jun 19, 2019
 *      Author: Bart
 */

#include <string.h>
#include <omnetpp.h>

using namespace omnetpp;

class HSS : public cSimpleModule {
protected:
    // The following redefined virtual function holds the algorithm.
    virtual void initialize() override;
    virtual void handleMessage(cMessage *msg) override;
};

// The module class needs to be registered with OMNeT++
Define_Module(HSS);

void HSS::initialize() {}

void HSS::handleMessage(cMessage *msg) {
    if (msg->getKind() == 2){
        cMessage *authDataRes = new cMessage("AuthenticationDataResponse", 2);
        send(authDataRes, "out");
        EV << "HSS generira autentifikacijski vektor te ga prosljedjuje MME-u\n";
    }
}
