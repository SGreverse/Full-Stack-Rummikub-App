using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RummiKubUI.UtilClasses
{
    public class DoublyLinkedImageList
    {
        public IconImage data;

        public DoublyLinkedImageList next;

        public DoublyLinkedImageList prev;

        public DoublyLinkedImageList(IconImage d)
        {
            this.data = d;
            next=prev = null;
        }
        public void SetPrev(IconImage d)
        {
            prev=new DoublyLinkedImageList(d);
            prev.next = this;
        }
        public void SetNext(IconImage d)
        {
            next=new DoublyLinkedImageList(d);
            next.prev = this;
        }

     
    }
}
